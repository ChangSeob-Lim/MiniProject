package com.example.test.mapper;

import java.util.List;
import java.util.Map;

import org.apache.ibatis.annotations.Mapper;

import com.example.test.param.EmployeeParam;

@Mapper
public interface ApiMapper {
	
	List<Map<String, Object>> getEmpInfo(EmployeeParam param);
}
