<template>
  <div class="login-container">
     <div class="login">
       <h2>Login</h2>
       <form @submit.prevent="LoginUser">
         <div class="form-group">
           <label for="username">Username</label>
           <input type="text" id="username" v-model="username" />
           <span class="error" v-if="errors.username">{{ errors.username }}</span>
         </div>
         <div class="form-group">
           <label for="password">Password</label>
           <input type="password" id="password" v-model="password" />
           <span class="error" v-if="errors.password">{{ errors.password }}</span>
         </div>
         <button type="submit">Login</button> OR  
          <router-link width="150px" class="btn btn"  to="/Acount/Register">Register</router-link>
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
      username: '',      
      password: '',
     
   
    };
  },
  async created() {
     
    console.log(this.$store);
  },
  methods: {
  
  validateusername() {
    this.errors.username = this.username ? '' : 'username is required';
  },
  
  validatePassword() {
    this.errors.password = this.password.length >= 8 ? '' : 'Password must be at least 8 characters';
  },
  
    
    async LoginUser() {
      this.validateusername();
     this.validatePassword();
if(Object.values(this.errors).every(error => !error)){
  try {
        const response = await axios.post('https://localhost:7181/api/account/login', {
          username: this.username,
          password: this.password,
        });
        console.log('login successful:', response.data);
      let data =response.data;
       localStorage.setItem('userData', data.data);
        localStorage.setItem('jwt', response.data.data.jwt);
         console.log(this.$store);  
        this.$store.commit('login');
        this.$router.push('/');
             } catch (error) {
        console.error('Registration failed:', error.response?.data);
      }
}
    
    }
  }
};
</script>


<style>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
}

.login {
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
