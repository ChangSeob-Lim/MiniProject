package com.server.service;

import org.apache.ibatis.annotations.Param;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.server.mapper.LoginMapper;

@Service
public class LoginService {
	
	@Autowired
	private LoginMapper mapper;
	
	public int login(@Param("user_id") String user_id, @Param("user_pw") String user_pw) {
		return mapper.login(user_id, user_pw);
	}
}
