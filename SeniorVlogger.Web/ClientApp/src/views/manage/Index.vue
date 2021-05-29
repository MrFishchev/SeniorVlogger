<template>
  <div class="wrapper">
    <router-view />
  </div>
</template>

<script>
export default {
  updated() {
    this.redirect()
  },
  beforeMount() {
    this.redirect()
  },
  methods:{
    redirect(){
      if(this.$route.path === '/manage')
        this.$router.replace('/manage/posts')
    }
  }
}
</script>