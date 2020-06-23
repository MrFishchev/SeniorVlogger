<template>
  <div class="wrapper">
    <router-view />
  </div>
</template>

<script>
export default {
  beforeMount() {
    if(this.$route.path === '/manage')
      this.$router.replace('/manage/posts')
  }
}
</script>