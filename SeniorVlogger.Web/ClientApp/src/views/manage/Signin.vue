<template>
  <div class="wrapper">
    <!-- TODO Loading  -->
    <form @submit.prevent="signin">
      <input
        v-model="credentials.username"
        type="text"
        placeholder="Username"
        required >
      <input
        v-model="credentials.password"
        type="password"
        placeholder="Password"
        required >
      <button type="submit">Submit</button>
    </form>
  </div>
</template>

<script>
import graphql from '@/graphql'
export default {
  data () {
    return {
      submitting: false,
      credentials: {
        username: '',
        password: ''
      }
    }
  },
  methods: {
    async signin () {
      this.submitting = true

      try {
        const token = await this.$apollo.mutate({
          mutation: graphql.auth.signin,
          variables: this.credentials
        }).then(({ data }) => data && data.authSignin)

        await this.$apolloHelpers.onLogin(token)
        this.$store.commit('setAuthorized', true)
        this.$router.replace({ path: '/manage' })
      } finally {
        this.submitting = false
      }
    }
  }
}
</script>
