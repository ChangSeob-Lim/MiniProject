package com.example.test.controller;

import java.util.List;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import com.example.test.item.EmployeeItem;
import com.example.test.param.EmployeeParam;
import com.example.test.service.ApiService;

@RestController
@RequestMapping(value="/getEmpInfo")
public class ApiController {
	
	@Autowired
	private ApiService service;
	
	@GetMapping(path="/hello")
	public String HelloSpringWorld()
	{
		return "Hello, spring world !!";
	}

	@RequestMapping(method=RequestMethod.GET)
	public List<Map<String, Object>> getEmpInfo(HttpServletRequest req, EmployeeParam param) {
		HttpSession session = req.getSession();
		
		System.out.println(session.getAttribute("id"));
		
		if(session.getAttribute("id") == null)
			return null;
		
		return service.getEmpInfo(param);
	}
	
	@PostMapping("/body")
	public String getEmpBody(@RequestBody EmployeeItem item) {
		return String.format("%d ", item.getEmpNo()) + item.geteName();
	}
	
	@RequestMapping(value="/{id}", method=RequestMethod.POST)
	public void update(@RequestBody EmployeeItem item) {
//		return service.update(item);
	}
	
	@RequestMapping(method=RequestMethod.PUT)
	public void insert(@RequestBody EmployeeItem item) {
//		return service.insert(item);
	}
	@RequestMapping(value="/{id}", method=RequestMethod.DELETE)
	public void delete(@RequestBody EmployeeItem item) {
//		return service.delete(item);
	}
}
