<template>
 <NavigationComponent />
    <div class="container">
     <div class="row">
     
        <router-view></router-view>
      
      
      </div>

   </div>
</template>
 
<script>
 import NavigationComponent from './components/NavigationComponent.vue'
export default { 
  name: 'App',
  components: {
  NavigationComponent
  }
}
</script>
<style> 
</style>
