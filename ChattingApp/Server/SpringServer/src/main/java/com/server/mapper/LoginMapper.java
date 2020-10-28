package com.server.mapper;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;

@Mapper
public interface LoginMapper {

	int login(@Param("user_id") String user_id, @Param("user_pw") String user_pw);
}
