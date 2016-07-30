-module(chatserver).
-export([start/1]).
-export([init/1, loop/1, accepter/2, client/2, client_loop/2]).

-record(chat, {socket, accepter, clients}).
-record(client, {socket, id, pid}).

start(Port) -> spawn(?MODULE, init, [Port]).

init(Port) ->
    {ok, S} = gen_tcp:listen(Port, [{packet, 2}, {active, false}]),
    A = spawn_link(?MODULE, accepter, [S, self()]),
    loop(#chat{socket=S, accepter=A, clients=[]}).

send_online_list(_ClientSock, []) ->
    ok;
send_online_list(ClientSock, Cs) ->
    [Client | Cs1] = Cs,			     
    io:format("players:~p~n", [Client#client.id]),
    S = lists:flatten(io_lib:fwrite("newclient,~p", [Client#client.id])),
    gen_tcp:send(ClientSock, S),
    send_online_list(ClientSock, Cs1).
			     
loop(Chat=#chat{accepter=A, clients=Cs}) ->
    receive
        {'new client', Client} ->
	    erlang:monitor(process, Client#client.pid),
        % 告知客户id
        S = lists:flatten(io_lib:fwrite("loginok,~p\r\n", [Client#client.id])),
        gen_tcp:send(Client#client.socket, S),
        % 告知新客户其它在线用户信息
	    send_online_list(Client#client.socket, Cs),
	    Cs1 = [Client | Cs],
	    broadcast(Cs1, ["newclient,~p", Client#client.id]),
	    loop(Chat#chat{clients=Cs1});
	{'DOWN', _, process, Pid, _Info} ->
	    case lists:keysearch(Pid, #client.pid, Cs) of
	        false -> loop(Chat);
		{value, Client} ->
		    self() ! {'lost client', Client},
		    loop(Chat)
	    end;
	{'lost client', Client} ->
	    broadcast(Cs, ["lostclient,~p", Client#client.id]),
	    gen_tcp:close(Client#client.socket),
	    loop(Chat#chat{clients=lists:delete(Client, Cs)});
	{message, Client, Msg} ->
	    broadcast(Cs,["~s,id,~p", Msg, Client#client.id]),
	    loop(Chat);
	refresh ->
	    A ! refresh,
	    lists:foreach(fun (#client{pid=CP}) -> CP ! refresh end, Cs),
	    ?MODULE:loop(Chat)
    end.

accepter(Sock, Server) ->
    {ok, Client} = gen_tcp:accept(Sock),
    spawn(?MODULE, client, [Client, Server]),
    receive
        refresh -> ?MODULE:accepter(Sock, Server)
    after 0 -> accepter(Sock, Server)
    end.

client(Sock, Server) ->
    random:seed(now()),
    Client = #client{socket=Sock, id=random:uniform(999999), pid=self()},
    Server ! {'new client', Client},
    % io:format("new client ~p~n", [Client]),
    client_loop(Client, Server).

client_loop(Client, Server) ->
    case gen_tcp:recv(Client#client.socket, 0) of
        {ok, Recv} ->
            lists:foreach(fun (S) -> Server ! {message, Client, S} end, string:tokens(Recv, "\r\n")),
	    client_loop(Client, Server);
        _Other ->
	       % io:format("recv other, ~p~n", [Client]), 
           Server ! {'lost client', Client}
    end.
    
broadcast(Clients, [Fmt | Args]) ->
    S = lists:flatten(io_lib:fwrite(Fmt, Args)),
    lists:foreach(fun (#client{socket=Sock}) -> 
        % io:format("broadcast=>Sock:~p Msg:~p~n", [Sock, S]), 
        gen_tcp:send(Sock, S) 
    end, Clients).