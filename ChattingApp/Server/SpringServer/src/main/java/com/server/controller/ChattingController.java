package com.server.controller;

import java.io.IOException;
import java.util.Collections;
import java.util.HashSet;
import java.util.Set;

import javax.websocket.OnClose;
import javax.websocket.OnError;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;

import org.springframework.stereotype.Component;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@Component
@ServerEndpoint("/chatting/server")
public class ChattingController {
	
	// WebSocket으로 브라우저가 접속하면 요청되는 함수
	@OnOpen
	public void handleOpen() {
		// 콘솔에 접속 로그를 출력한다.
		System.out.println("client is now connected...");
	}
	
	// WebSocket으로 메시지가 오면 요청되는 함수
	@OnMessage
	public String handleMessage(String message) {
		// 메시지 내용을 콘솔에 출력한다.
		System.out.println("receive from client : " + message);
		// 에코 메시지를 작성한다.
		String replymessage = "echo " + message;
		// 에코 메시지를 콘솔에 출력한다.
		System.out.println("send to client : "+replymessage);
		// 에코 메시지를 브라우저에 보낸다.
		return replymessage;
	}
	
	// WebSocket과 브라우저가 접속이 끊기면 요청되는 함수
	@OnClose
	public void handleClose() {
		// 콘솔에 접속 끊김 로그를 출력한다.
		System.out.println("client is now disconnected...");
	}
	
	// WebSocket과 브라우저 간에 통신 에러가 발생하면 요청되는 함수.
	@OnError
	public void handleError(Throwable t) {
		// 콘솔에 에러를 표시한다.
		t.printStackTrace();
	}
	
/*
	public static Set<Session> sessionUsers = Collections.synchronizedSet(new HashSet<Session>());
	
	@OnOpen
	public void handleOpen(Session userSession) {
		System.out.println("WebSocket : Client Session is Open. ID is "+ userSession.getId());
		sessionUsers.add(userSession);
		System.out.println("연결된 세션 수" + sessionUsers.size());
	}

	// 자신에게 메시지 전달 실험
//	@OnMessage
//	public String handleMessage(String message) throws IOException {
//		Iterator<Session> iterator = sessionUsers.iterator();
//		System.out.println("WebSocket : Send message to all client.");
//		
//		System.out.println(message);
//
//		return message;
//	}
	
	@OnMessage
	public void handleMessage(Session userSession, String message) throws IOException {
		System.out.println(userSession.getId() + " WebSocket : "+ message);
		System.out.println("WebSocket : Send message to all client.");
		if(sessionUsers.size() > 0) {
			for(Session sess : sessionUsers)
			{
				System.out.println("Can Send to " + sess.getId() + "?");
				
				if(sess == userSession)
					continue;
				
				sess.getBasicRemote().sendText(userSession.getId() + ": " + message);
			}
		} else {
			System.out.println("WebSocket : Here is no registered destination.");
		}
	}

	@OnClose
	public void handleClose(Session session) {
		System.out.println("WebSocket : Session remove complete. ID is "+session.getId());
		sessionUsers.remove(session);
		System.out.println("연결된 세션 수" + sessionUsers.size());
	}
*/
}
