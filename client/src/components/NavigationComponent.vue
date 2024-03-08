 <template>
 <nav class="navbar navbar-expand-lg navbar-light bg-light">
  <a class="navbar-brand" href="#">MS </a>
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>
  <div class="collapse navbar-collapse" id="navbarNav">
    <ul class="navbar-nav">      
      <li class="nav-item">
        <router-link class="nav-link" to="/">Home</router-link>
      </li>     
    </ul>
      
  </div>
  <div class="nav-item ml-auto">  
      <router-link  v-if="!isAuthenticated" class="btn btn-primary" to="/Acount/Login">Login</router-link>
  </div>  
  <div  class=" ml-auto" v-if="isAuthenticated"><button @click="logout">Logout</button></div>
    
</nav>
</template>

<script>
 
export default {
  computed: {
    isAuthenticated: function () {
      console.log('isAuthenticated',this.$store);      // Access the state based on your chosen approach (Vuex or Local Storage)
      if (this.$store) {  
        return this.$store.state.isAuthenticated;
      } else { 
        return localStorage.getItem('isAuthenticated') === 'true';
      }
    },
    username: function () {
  
      return this.$store?.state.user?.username || localStorage.getItem('username');
    },
  },
  methods: {
    logout() {
       
      if (this.$store) {
        this.$store.commit('logout');
        this.$router.push('/Acount/Login');
        
      } else {
        localStorage.removeItem('isAuthenticated');
      }
      // Redirect to login page or handle logout actions
    },
  },
};
</script>
<style scoped>
a.active{
 background:#bbb
}
</style>
