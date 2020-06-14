<template>
  <div class="wrapper">
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
    <span class="danger" v-if="error">{{error}}</span>
  </div>
</template>

<script>
export default {
  data () {
    return {
      submitting: false,
      error: null,
      credentials: {
        username: '',
        password: ''
      }
    }
  },
  methods: {
    signin () {
        this.$store.dispatch('LOGIN', this.credentials)
            .then(data => {
                if (data.success)
                    this.$router.replace('/manage')
                else
                    this.error = data.message
            })
    }
  }
}
</script>
