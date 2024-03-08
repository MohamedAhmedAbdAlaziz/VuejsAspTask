import { createStore } from 'vuex'

export default createStore({
  state() {
    return {
      isAuthenticated: localStorage.getItem('jwt') !=null && localStorage.getItem('jwt') !='' ? true :false, // Initial state from local storage
    };
  },
  mutations: {
    setUser(state, user, token) {
      state.user = user;
      localStorage.setItem('jwt', token); 
    },
    logout(state) {
      state.isAuthenticated = false;
      localStorage.removeItem('jwt'); 
   
    },
    login(state) {
      state.isAuthenticated = true;
    },
  },
  actions: {
    fetchUser({ commit }) {
      setTimeout(() => {
        const user = { name: 'John Doe', email: 'john.doe@example.com' };
        commit('setUser', user);
      }, 1000);
    },
     login({ commit }, loginData) {
      setTimeout(() => {
        const user = { name: loginData.username, email: loginData.email };
        commit('setUser', user);
      }, 1000);
    },
  },
});
