<template>
  <div class="wrapper col-lg-7 col-md-10 col-sm-12" id="user-create">
    <div class="user p-5">
      <form class="form" @submit.prevent="Publish" enctype="multipart/form-data">
        <div class="form-group" v-if="!editMode">
          <label for="email">Email</label>
          <input type="email" placeholder="Email" id="email" v-model="user.email" class="form-control" required/>
        </div>
        <div class="form-group">
          <label for="password">Password</label>
          <input type="password" placeholder="Password" id="password" v-model="user.password" class="form-control" required/>
        </div>
        <div class="form-group">
          <label for="password2">Confirm Password</label>
          <input type="password" placeholder="Confirm Password" id="password2" v-model="user.confirmPassword" class="form-control" required/>
          <span v-show="passwordError" class="error text-danger">Password and Confirm Password are different</span>
        </div>
<!--        <div class="form-group">-->
<!--          <select class="form-control" v-model="user.role" required>-->
<!--            <option disabled value="" v-show="editMode">Please select one</option>-->
<!--            <option value="0" selected>User</option>-->
<!--            <option value="1">Author</option>-->
<!--            <option value="2">Administrator</option>-->
<!--          </select>-->
<!--        </div>-->

        <div class="buttons mb-5">
          <button :disabled="isSubmitting" class="btn btn-success w-25 text-white" type="submit">{{ editMode ? 'Update User' : 'Create User'}}</button>
          <router-link to="/manage/users" tag="button" class="btn btn-danger w-25 text-white" @click.prevent="this.$router.push('/')">Cancel</router-link>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  data () {
    return {
      editMode: false,
      isSubmitting: false,
      passwordError: false,
      user: {
        id: 0,
        email: '',
        password: '',
        confirmPassword: ''
      }
    }
  },

  beforeMount() {
    let id = this.$route.params.id
    if(id){
      this.user.id = id
      this.editMode = true
    }
  },

  methods: {
    async Publish() {
      if(this.user.password !== this.user.confirmPassword){
        this.passwordError = true
        return
      }
      else{
        this.passwordError = false
      }
      
      this.isSubmitting = true
      if(this.editMode){
        this.UpdateUser()
        return
      }

      this.$api.post('/api/user', this.user)
          .then(res => this.$router.push({ path: '/manage/users' }))
          .catch(error => {
            this.$notify({
              type: 'error',
              title: 'Error',
              text: 'Cannot save the user',
              group: 'app',
            })
            this.isSubmitting = false
          })
    },

    UpdateUser() {
      this.$api.put(`/api/user/${this.user.id}`, this.user)
          .then(res => this.$router.push({path: '/manage/users'}))
          .catch(error => {
            this.$notify({
              type: 'error',
              title: 'Error',
              text: 'Cannot update the user',
              group: 'app',
            })
            this.isSubmitting = false
          })
    }

  }
}
</script>

<style lang="sass">
#user-create
  margin: 0 auto
  .buttons
    display: flex
    justify-content: space-evenly
    align-items: center
  .error
    position: relative
    top: +5px
    left: +10px
    
</style>