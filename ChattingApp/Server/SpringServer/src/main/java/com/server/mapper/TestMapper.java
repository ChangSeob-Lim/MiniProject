package com.server.mapper;

import java.util.List;
import java.util.Map;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;

import com.server.item.TestItem;

@Mapper
public interface TestMapper {
	// SELECT (GET)
		List<TestItem> selectAll();
		List<Map<String, Object>> select(String param);
		
		// UPDATE (POST)
		void update(@Param("tid") String tid, @Param("code") String code, @Param("name") String name);
		
		// INSERT (PUT)
		void insert(@Param("code") String code, @Param("name") String name);
		
		// DELETE (DELETE)
		void delete(@Param("tid") String tid);
}