package com.server.controller;

import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import com.server.item.TestItem;
import com.server.service.TestService;

@RestController
public class TestController {
	
	@Autowired
	private TestService service;
	
	@GetMapping(path="/hello")
	public String HelloSpringWorld()
	{
		return "Hello, spring world !!";
	}
	
	@RequestMapping(method=RequestMethod.GET)
	public List<TestItem> selectAll() {
		
		return service.selectAll();
	}
	
	@RequestMapping(value="/{pid}", method=RequestMethod.GET)
	public List<Map<String, Object>> select(@PathVariable("pid") String param) {
		
		return service.select(param);
	}
	
	@RequestMapping(value="/{id}", method=RequestMethod.POST)
	public void update(@RequestBody TestItem item) {
		service.update(item);
	}
	
	@RequestMapping(method=RequestMethod.PUT)
	public void insert(@RequestBody TestItem item) {
		service.insert(item);
	}
	
	@RequestMapping(value="/{id}", method=RequestMethod.DELETE)
	public void delete(@RequestBody TestItem item) {
		service.delete(item);
	}
}
