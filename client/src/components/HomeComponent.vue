<template>
   <div width="100%">
    <router-link width="150px" style="left: 0px;" class="btn btn-primary"  to="/Employee">Create New Employee</router-link>
    <h2>Employee List</h2>
    <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>email</th>
          <th>phone Number</th>
          <th>GraduationStatus</th>
          <th>Image</th>

          <th>Action</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="employee in employees" :key="employee.id">
          <td>{{ employee.name }}</td>
          <td>{{ employee.email }}</td>
          <td>{{ employee.phoneNumber }}</td>
          <td>{{ employee.GraduationStatus == 1 ? 'Undergrad':'graduate'  }}</td>
          <td><img :src=employee.imageUrI style="max-width: 100px;"></td>
          <td>
            <button @click="editEmployee(employee)">Edit</button>
            <button class="btn btn-danger" @click="deleteEmployee(employee.id)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
 
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      employees: []
    };
  },
  mounted() {
    this.fetchEmployees();
  },
  methods: {
    async fetchEmployees() {
      try {
        const response = await axios.get('https://localhost:7181/api/Employees',{
            headers: {
             Authorization: `Bearer ${localStorage.getItem('jwt')}` // Include JWT token in Authorization header
          }
          });
        this.employees = response.data.data;
      } catch (error) {
        console.error('Failed to fetch employees:', error);
      }
    },
    editEmployee(employee) {
      this.$router.push(`/Employee/${employee.id}`);
      // Implement edit functionality
      console.log('Editing employee:', employee);
    },
    async deleteEmployee(employeeId) {
      let response=Object;
      try {
          response =  await axios.delete(`https://localhost:7181/api/Employees/${employeeId}`,
        {
            headers: {
             Authorization: `Bearer ${localStorage.getItem('jwt')}` // Include JWT token in Authorization header
          }
          }
        );
        // Remove the deleted employee from the list
        this.employees = this.employees.filter(employee => employee.id !== employeeId);
      } catch (error) {

        if(!error.response?.data.status){
             alert(error.response.data?.message);
        }
        console.error('Failed to delete employee:', error);
      }
      console.log('response',response);
    }
  }
};
</script>

<style>
/* Add your CSS styles here */
table {
  width: 100%;
  border-collapse: collapse;
}
th, td {
  padding: 8px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}
th {
  background-color: #f2f2f2;
}
button {
  margin-right: 5px;
}
</style>