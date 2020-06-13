<template>
  <div class="wrapper">
    <menu>
      <li><router-link to="/manage/posts">Posts</router-link></li>
    </menu>

    <router-view />
  </div>
</template>

<script>
export default {
  computed: {
    authorized () {
      // return this.$store.state.authorized || this.$apolloHelpers.getToken()
    }
  },
  mounted () {
    // if (!this.authorized) {
    //   this.$router.push({ path: '/manage/signin' })
    // }
  }
}
</script>
