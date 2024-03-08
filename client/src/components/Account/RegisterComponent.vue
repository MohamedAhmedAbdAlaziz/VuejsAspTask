<template>
    <div class="register-container">
        <div class="register">
            <h2>Registration</h2>
      <form @submit.prevent="registerUser">
        <div class="form-group">
            <label for="firstname">First Name</label>
            <input type="text" id="firstname" class="form-control" v-model="firstname" @blur="validatefirstname" />
          
            <span class="error" v-if="errors.firstname">{{ errors.firstname }}</span>
          </div>
          <div class="form-group">
            <label for="lastname">last Name</label>
            <input type="text" id="lastname" class="form-control" v-model="lastname" @blur="validatelastname" />
          
            <span class="error" v-if="errors.lastname">{{ errors.lastname }}</span>
          </div>
          <div class="form-group">
            <label for="email">Email</label>
            <input type="email" id="email" class="form-control" v-model="email" @blur="validateEmail" />
           
            <span class="error" v-if="errors.email">{{ errors.email }}</span>
          </div>
        <div>
          <label for="password">Password</label>
          <input type="password" id="password" class="form-control" v-model="password" />
          <span class="error" v-if="errors.password">{{ errors.password }}</span>
          
        </div>
        <div>
          <label for="role">Role</label>
          <select id="role" v-model="selectedRole" class="form-control">
            <option v-for="role in roles" :value="role.id" :key="role.id">{{ role.name }}</option>
          </select>
          <span class="error" v-if="errors.selectedRole">{{ errors.selectedRole }}</span>

        </div>
        <div>
          <button type="submit">Register</button>
        </div>
      </form>
        </div>
   
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        errors: {},
        firstname: '',
        lastname: '',
        email: '',
        password: '',
        selectedRole: '',
        roles: [],
     
      };
    },
    async created() {
      // Fetch roles when the component is created
      await this.fetchRoles();
      console.log(this.$store);
    },
    methods: {
     validatefirstname() {
      this.errors.firstname = this.firstname ? '' : 'firstname is required';
    },
    validatelastname() {
      this.errors.lastname = this.lastname ? '' : 'lastname is required';
    },
    validateEmail() {
      const re = /\S+@\S+\.\S+/;
      this.errors.email = re.test(this.email) ? '' : 'Invalid email address';
    },
    validatePassword() {
      this.errors.password = this.password.length >= 8 ? '' : 'Password must be at least 8 characters';
    },
    validateConfirmPassword() {
      this.errors.selectedRole = this.selectedRole ? '' : 'role is required';
    },
      async fetchRoles() {
        try {
          const response = await axios.get('https://localhost:7181/api/account/AllRoles');
          this.roles = response.data.data;
        } catch (error) {
          console.error('Failed to fetch roles:', error);
        }
      },
      async registerUser() {
        this.validatefirstname();
        this.validatelastname();
       this.validateEmail();
       this.validatePassword();
       this.validateConfirmPassword();
if(Object.values(this.errors).every(error => !error)){
    try {
          const response = await axios.post('https://localhost:7181/api/account/register', {
            firstname: this.firstname,
            lastname: this.lastname,
            email: this.email,
            password: this.password,
            role: this.selectedRole
          });   
        console.log( response.data.data.jwt);
        localStorage.setItem('jwt', response.data.data.jwt);
        this.$store.commit('login');
        this.$router.push('/');
         console.log('Registration successful:', response.data);
            console.log(this.$store);
           
         } catch (error) {
          alert(error.response.data?.errors[0]);
          console.error('Registration failed:', error.response.data);
        
        }
}
      
      }
    }
  };
  </script>
  
 
  <style>
  .register-container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
  }
  
  .register {
     width: 500px;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
  }
  
  .form-group {
    margin-bottom: 20px;
  }
  
  label {
    display: block;
    margin-bottom: 5px;
  }
  
  input[type="text"],
  input[type="email"],
  input[type="password"] {
    width: 100%;
    padding: 10px;
    font-size: 16px;
  }
  
  button {
    width: 100%;
    padding: 10px 20px;
    font-size: 16px;
    background-color: #007bff;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
  }
  
  button:hover {
    background-color: #0056b3;
  }
  
  .error {
    color: red;
  }
  </style>
  