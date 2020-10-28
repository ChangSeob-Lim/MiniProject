package com.server.controller;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import com.server.item.LoginItem;
import com.server.service.LoginService;

@RestController
public class LoginController {
	
	private LoginService service;

	@Autowired
	public LoginController(LoginService service) {
		this.service = service;
	}
	
	@RequestMapping(value="/login", method=RequestMethod.POST)
	public String canLogin(HttpServletRequest req, @RequestBody LoginItem item) {
		
		HttpSession session = req.getSession();
		
		int check = service.login(item.getUser_id(), item.getUser_pw());
		
		if(check > 0)
		{
			return "T";
		}
		else
			return "F";
	}
	
	@GetMapping("/logout")
	public String Logout(HttpServletRequest req) {
		HttpSession session = req.getSession();
		
		session.invalidate();
		
		return "삭제";
	}
}
