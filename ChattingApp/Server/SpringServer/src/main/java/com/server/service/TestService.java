package com.server.service;

import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.server.item.TestItem;
import com.server.mapper.TestMapper;

@Service
public class TestService {
	
	@Autowired
	private TestMapper mapper;
	
	public List<TestItem> selectAll() {
		return mapper.selectAll();
	}
	
	public List<Map<String, Object>> select(String param) {
		return mapper.select(param);
	}
	
	public void update(TestItem item) {
		mapper.update(item.getTid(), item.getCode(), item.getName());
	}
	
	public void insert(TestItem item) {
		mapper.insert(item.getCode(), item.getCode());
	}
	
	public void delete(TestItem item) {
		mapper.delete(item.getTid());
	}
}
