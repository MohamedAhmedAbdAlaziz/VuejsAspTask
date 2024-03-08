import App from './App.vue'
import "bootstrap/dist/css/bootstrap.css"
import HomeComponent from './components/HomeComponent.vue'
import EmployeeComponent from './components/EmployeeComponent.vue'
import RegisterComponent from './components/Account/RegisterComponent.vue'
import LoginComponent from './components/Account/LoginComponent.vue'
import { createRouter } from 'vue-router'
 import {createApp} from 'vue'
import { createWebHistory } from 'vue-router'
//import { store } from './store/store'
import store from './store/store'
import AccessDeniedComponent from './components/Shared/AccessDeniedComponent.vue';

//Vue.use(VueRouter)

const routes = [
  { path: '/', component: HomeComponent },
  { path: '/Employee/:id', component: EmployeeComponent },
  { path: '/Employee/', component: EmployeeComponent },
  { path: '/Acount/Register', component: RegisterComponent },
  { path: '/Acount/Login', component: LoginComponent },
  { path: '/AccessDenied', component: AccessDeniedComponent },
 
]
const router = new createRouter({
    history:createWebHistory(),
    routes:routes, // short for `routes: routes`
    linkActiveClass:'active'
  });
var app= createApp(App);
app.use(store);

app.use(router);
app.mount('#app');
import "bootstrap/dist/js/bootstrap.js";
// new Vue({
//     router,
//     render: h => h(App),
//   }).$mount('#app')