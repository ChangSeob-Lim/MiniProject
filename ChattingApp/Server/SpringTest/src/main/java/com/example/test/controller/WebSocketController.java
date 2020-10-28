package com.example.test.controller;

import java.io.IOException;
import java.util.Collections;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;

import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;

import org.springframework.web.bind.annotation.RestController;
import net.minidev.json.JSONObject;

@RestController
@ServerEndpoint("/websocket")
public class WebSocketController {
	
//	static List<Session> sessionUsers = Collections.synchronizedList(new ArrayList<>());
//	
//	// 웹 소켓 연결시 호출
//	@OnOpen
//	public void handleOpen(Session userSession) {
//		System.out.println("Web Socket Open()");
//		System.out.println("Client Session ID is " + userSession.getId());
//		sessionUsers.add(userSession); // 웹 소켓 연결시 세션 리스트에 추가
//	}
//	
//	// 웹 소켓 메시지 수신시 호출
//	@OnMessage
//	public void handleMessage(String message) throws IOException{
//		System.out.println("수신 메시지 : " + message);
//		
//		//세션리스트에게 데이터를 보낸다
//		Iterator<Session> iterator = sessionUsers.iterator();
//		
//		if(sessionUsers.Size() > 0) {
//			while(iterator.hasNext()) {
//				iterator.next().getBasicRemote().sendText(JsonbHttpMessageConverter(message, "message", "event"));
//			}
//		}
//		
//		
//	}
	static Set<Session> sessionUsers = Collections.synchronizedSet(new HashSet<Session>());
	
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
	public void handleMessage(String message) throws IOException {
		Iterator<Session> iterator = sessionUsers.iterator();
		System.out.println("WebSocket : Send message to all client.");
		if(sessionUsers.size() > 0) {
			while(iterator.hasNext()) {
				iterator.next().getBasicRemote().
						sendText(message);
//	                   sendText(JSONConverter(message, "message", "event"));
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
//	public String JSONConverter(String message, String command, String type) {
//		JSONObject jsonObject = new JSONObject();
//		jsonObject.put("type", type);
//		jsonObject.put("command", command);
//		jsonObject.put("message", message);
//		return jsonObject.toString();
//	}

}
