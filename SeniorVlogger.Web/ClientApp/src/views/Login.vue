<template>
  <div id="manage" class="container">
    <div class="col-lg-6 col-md-10 col-sm-12 m-auto border border-primary rounded p-5 shadow">
      <form @submit.prevent="signIn">

       <div class="form-group">
          <label for="email">Email</label>
          <input type="email" v-model="credentials.username" spellcheck="false"
                class="form-control" id="email" placeholder="Someone@example.com">
          <small id="emailHelp" class="form-text text-danger">{{error}}</small>
        </div>

        <div class="form-group">
          <label for="password">Password</label>
          <input type="password" v-model="credentials.password"
                class="form-control" id="password" placeholder="Password">
        </div>

        <div class="form-group">
          <div class="form-check">
            <input class="form-check-input" v-model="credentials.remember"
                  type="checkbox" id="gridCheck">
            <label class="form-check-label" for="gridCheck">
              Remember Me
            </label>
          </div>
        </div>
        <button :class="{'btn mt-3': true, 'btn-primary': !submitting, 'btn-light': submitting}" 
                type="submit">
            Submit
        </button>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  data () {
    return {
      submitting: false,
      error: '',

      credentials: {
        username: '',
        password: '',
        remember: false
      }
    }
  },
  methods: {
      signIn() {
        if(this.submitting) return
        this.submitting = true
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

<style lang="sass" scoped>
  #manage
    display: flex
    align-items: center
    height: 100vh
    .btn
      width: 100%
</style>
