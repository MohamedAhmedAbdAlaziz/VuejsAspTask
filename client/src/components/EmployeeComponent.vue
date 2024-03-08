<template>
  <div>
    <h2>{{ mode === 'create' ? 'Create Employee' : 'Edit Employee' }}</h2>
    <form @submit.prevent="submitForm">
      <div class="form-group">
        <label for="name">Name:</label>
        <input  class="form-control" type="text" id="name" v-model="employee.name"  >
        <span class="error" v-if="errors.name">{{ errors.name }}</span>
    
      </div>
      <div class="form-group">
      <label for="name">Email</label>
        <input  class="form-control" type="text" id="name" v-model="employee.email" >
        <span class="error" v-if="errors.email">{{ errors.email }}</span>
     </div>
     <div class="form-group">
        <label for="email">Phone:</label>
        <input  class="form-control" type="text" id="hhone" v-model="employee.phoneNumber" >
        <span class="error" v-if="errors.phoneNumber">{{ errors.phoneNumber }}</span>
   
      </div>
      <div class="form-group">
          <label for="role">graduation Status</label>
          <select  id="role" v-model="employee.graduationStatus" class="form-control">
            <option value="0" >underGrad</option>
            <option value="1">Graduate</option>
          </select>
          <span class="error" v-if="errors.graduationStatus">{{ errors.graduationStatus }}</span>
        </div>
        <!-- <div class="form-group">
        <label for="ImageUrI">ImageUrI</label>
        <input class="form-control" type="text" id="ImageUrI" v-model="employee.ImageUrI" >
        <span class="error" v-if="errors.imageUrI">{{ errors.imageUrI }}</span>
    
      </div> -->
              <div>
            <h2>Upload Image</h2>
            <input id="photo" type="file" ref="fileInput" @change="handleFileUpload">
            <!-- <img :src="imageUrl" v-if="imageUrl" style="max-width: 300px;"> -->
            
          </div>

      <button type="submit">{{ mode === 'create' ? 'Create' : 'Update' }}</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      errors: {},
      mode: 'create', // Mode can be 'create' or 'edit'
      employee: {
        name: '',
        email: '',
        phoneNumber: '',
        graduationStatus: '',
        imageUrI:'',
        photo:null
      }
    };
  },
  mounted() {
    if (this.$route.params.id) {
      // If an employee ID is provided in the route params, switch to edit mode
      this.mode = 'edit';
      this.fetchEmployee();
    }
  },
  methods: {
    handleFileUpload(event) {
      this.employee.photo = event.target.files[0];
    },
    async fetchEmployee() {
      try {
        const response = await axios.get(`https://localhost:7181/api/Employees/${this.$route.params.id}`,
        
        {
            headers: {
             Authorization: `Bearer ${localStorage.getItem('jwt')}` // Include JWT token in Authorization header
          }
          }
        );
        
        this.employee = response.data.data;
      } catch (error) {
        console.log(error);
        this.$router.push('/AccessDenied');
        console.error('Failed to fetch employee:', error);
      }
    },
    validateName() {
    this.errors.name = this.employee.name ? '' : 'username is required';
  },
  
  validatePhoneNumber() {
    this.errors.phoneNumber = this.employee.phoneNumber  ? '' : 'phoneNumber is required';
  },
  
  validateEmail() {
    this.errors.email = this.employee.email ? '' : 'email is required';
  },
  validateGraduationStatus() {
    this.errors.graduationStatus = this.employee.graduationStatus  ? '' : 'graduationStatus is required';
  },
  
    async submitForm() {
      this.validateName();
      this.validateEmail();
      this.validatePhoneNumber();
      this.validateGraduationStatus();
 
if(Object.values(this.errors).every(error => !error)){
      try {
        const formData = new FormData();
        formData.append('name', this.employee.name);
        formData.append('email', this.employee.email);
        formData.append('phoneNumber', this.employee.phoneNumber);
        formData.append('graduationStatus', this.employee.graduationStatus);
        formData.append('photo', this.employee.photo );
        if (this.mode === 'create') {
          await axios.post('https://localhost:7181/api/Employees', formData,
          {
          headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${localStorage.getItem('jwt')}` // Include JWT token in Authorization header
     
          }
        });
        } else {
          await axios.put(`https://localhost:7181/api/Employees/${this.$route.params.id}`,
          formData,{
            headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${localStorage.getItem('jwt')}` // Include JWT token in Authorization header
     
          }
          });
        }
        // Redirect to employee list after submission
        this.$router.push('/employees');
      } catch (error) {
        console.error('Failed to submit form:', error.errors);
      }
    }
    }
  }
};
</script>

<style>
/* Add your CSS styles here */
label {
  display: block;
  margin-bottom: 5px;
}
input {
  width: 100%;
  padding: 8px;
  margin-bottom: 10px;
}
button {
  padding: 8px 16px;
  background-color: #007bff;
  color: #fff;
  border: none;
  cursor: pointer;
}
</style>
