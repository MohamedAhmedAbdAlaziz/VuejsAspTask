import { createStore } from 'vuex';

export default createStore({
  state() {
    return {
      user: null,
      isAuthenticated:
        localStorage.getItem('jwt') != null && localStorage.getItem('jwt') != ''
          ? true
          : false, // Initial state from local storage
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
  actions: {},
});
