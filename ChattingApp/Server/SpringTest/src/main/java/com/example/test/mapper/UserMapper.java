package com.example.test.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.Update;
import com.example.test.item.UserItem;

@Mapper
public interface UserMapper {
	
	@Select("SELECT * FROM USER WHERE id=#{id}")
	UserItem getUser(@Param("id") String id);
	
	@Select("SELECT * FROM USER")
	List<UserItem> getUserList();
	
	@Insert("INSERT INTO USER VALUES (#{id}, #{password}, #{name})")
	int insertUser(@Param("id") String id, @Param("password") String password, @Param("name") String name);
	
	@Update("UPDATE USER SET password=#{password}, name=#{name} WHERE id=#{id}")
	int updateUser(@Param("id") String id, @Param("password") String password, @Param("name") String name);
	
	@Delete("DELETE FROM USER WHERE id=#{id}")
	int deleteUser(@Param("id") String id);
	
//	@Select("SELECT COUNT(*) FROM USER WHERE id=#{id} AND password=#{password}")
//	int CanLogin(@Param("id") String id, @Param("password") String password);
	
	@Select("SELECT COUNT(*) FROM USER WHERE id=#{id} AND password=#{password}")
	int CanLogin(@Param("id") String id, @Param("password") String password);
}
