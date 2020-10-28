package com.example.test.item;

public class EmployeeItem {
		
	public EmployeeItem() {
		super();
	}
	public EmployeeItem(int empNo, String eName) {
		super();
		this.empNo = empNo;
		this.eName = eName;
	}
	
	private int empNo;
	private String eName;
	public int getEmpNo() {
		return empNo;
	}
	public void setEmpNo(int empNo) {
		this.empNo = empNo;
	}
	public void setEName(String eName) {
		this.eName = eName;
	}
	public String geteName() {
		return eName;
	}
}
