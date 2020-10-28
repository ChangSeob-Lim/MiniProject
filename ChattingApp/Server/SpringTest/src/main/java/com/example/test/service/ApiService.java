package com.example.test.service;

import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.example.test.mapper.ApiMapper;
import com.example.test.param.EmployeeParam;

@Service
public class ApiService {
	
	@Autowired
	private ApiMapper mapper;
	
	public List<Map<String, Object>> getEmpInfo(EmployeeParam param) {
		return mapper.getEmpInfo(param);
	}
}
