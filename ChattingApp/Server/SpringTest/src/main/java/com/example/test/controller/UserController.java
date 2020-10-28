package com.example.test.controller;

import java.util.ArrayList;
import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import com.example.test.item.LoginItem;
import com.example.test.item.UserItem;
import com.example.test.mapper.UserMapper;

@RestController
public class UserController {


	public static List<String> loginUserList = null;
	private UserMapper mapper;

	// private Map<String, UserItem> userMap;
	
	public UserController(UserMapper mapper) {
		this.mapper = mapper;
		loginUserList = new ArrayList<>();
	}

	// @PostConstruct
	// public void Init() {
	// userMap = new HashMap<String, UserItem>();
	// userMap.put("1", new UserItem("1", "1", "A"));
	// userMap.put("2", new UserItem("2", "2", "B"));
	// userMap.put("3", new UserItem("3", "3", "C"));
	// }
	
	@RequestMapping(value="/login", method=RequestMethod.POST)
	public String canLogin(HttpServletRequest req, @RequestBody LoginItem item) {
		
		HttpSession session = req.getSession();
		
		int check = mapper.CanLogin(item.getUserName(), item.getUserPassword());
		
		if(check > 0)
		{ 
//			if(loginUserList.contains(item.getUserName()))
//				return "이미 로그인";
//			else {
//				loginUserList.add(item.getUserName());
//			}
			System.out.println(session.getAttribute("check"));
			if(session.getAttribute("check") != null)
				return "이미 로그인";
			
			session.setAttribute("check", item);
//			return "로그인 성공";
			return "T";
		}
		else
		{
//			return "로그인 실패";
			return "F";
		}
	}
	
	@GetMapping("/logout")
	public String Logout(HttpServletRequest req) {
		HttpSession session = req.getSession();
		
		session.removeAttribute("check");
		session.invalidate();
		
		return "삭제";
	}

	@GetMapping("/user/{id}")
	public UserItem getUser(HttpServletRequest req, @PathVariable("id") String id) {
		// return userMap.get(id);
		
		HttpSession session = req.getSession();
		
		session.setAttribute("id", id);
		
		return mapper.getUser(id);
	}

	@GetMapping("/user")
	public List<UserItem> getUserList() {
		// return new ArrayList<UserItem>(userMap.values());
		return mapper.getUserList();
	}

	@PutMapping("/user/{id}")
	public void putUser(@PathVariable("id") String id, @RequestParam("password") String password,
			@RequestParam("name") String name) {
		// UserItem userItem = new UserItem(id, password, name);
		// userMap.put(id, userItem);
		mapper.insertUser(id, password, name);
	}

	@PostMapping("/user/{id}")
	public void postUser(@PathVariable("id") String id, @RequestParam("password") String password,
			@RequestParam("name") String name) {
		// UserItem userItem = userMap.get(id);
		// userItem.setPassword(password);
		// userItem.setName(name);
		mapper.updateUser(id, password, name);
	}

	@DeleteMapping("/user/{id}")
	public void deleteUser(@PathVariable("id") String id) {
		// userMap.remove(id);
		mapper.deleteUser(id);
	}
	
	@GetMapping("/sessionMake")
	public String sessionMake(HttpServletRequest req, @RequestParam("userId") String userId) {
		HttpSession session = req.getSession();
		
		session.setAttribute("userId", userId);
		
		String Id = (String)session.getAttribute("userId");
		
		System.out.println("유저 아이디: " + Id);
		
		System.out.println("유저 아이디: " + userId);
		
		System.out.println("유저 아이디: " + session.getId());
		
		//session.invalidate();
		
		return userId + "생성";
	}
	
	@GetMapping("/sessionDelete")
	public String sessionDelete(HttpServletRequest req) {
		HttpSession session = req.getSession();
		
		session.invalidate();
		
		return "삭제";
	}
	
	@GetMapping("/sessionTest")
	public String sessionTest(HttpServletRequest req) {
		HttpSession session = req.getSession();
		
		String userId = (String)session.getAttribute("userId");
		
		System.out.println("유저 아이디: " + userId);
		
		userId = (String)session.getAttribute("id");
		
		System.out.println("유저 아이디: " + userId);
		
		System.out.println(session.getAttribute("check"));
		
		return userId;
		
		//session.invalidate();
	}
}
