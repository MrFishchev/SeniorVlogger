<template>
  <footer class="wrapper">
    <router-link to="/blog" v-if="NeedReadBlogButton"> 
      <button
        type="button"
        class="btn">
        Read Blog
      </button>
    </router-link>
    <button type="button" class="btn" v-else @click="Subscribe">Subscribe</button>

    <div class="icon-wrapper">
      <a
        href="https://www.linkedin.com/in/mrfishchev"
        target="_blank"
        class="icon">
        <div>
          <LinkedInIcon />
        </div>
      </a>

      <a
        href="https://www.instagram.com/mrfishchev"
        target="_blank"
        class="icon">
        <div>
          <InstagramIcon />
        </div>
      </a>

      <a
        href="https://github.com/mrfishchev"
        target="_blank"
        class="icon">
        <div>
          <GithubIcon />
        </div>
      </a>
    </div>
    <div class="copyright">
      Aleksey Fishchev <span>&copy; 2020</span>
    </div>
  </footer>
</template>

<style lang="sass">
    @import '@/assets/_colors'
    footer.wrapper
        padding: 70px 0 50px 0
        background: $base-dark
        position: relative
        .btn
            position: absolute
            height: 40px
            top: -20px
            width: 160px
            left: calc( 50% - 80px)
            background-image: linear-gradient(to right,$neon-light,$neon)
            background-size: 200% auto
            border-radius: 0
            border: none
            display: flex
            justify-content: space-evenly
            align-items: center
            transition: color 0.2s
            &:hover
                border: none
                background: $font-white
                color: $neon-light
        .icon-wrapper
            display: flex
            justify-content: center
            .icon
                margin-right: 2vw
                div
                    width: 55px
                    height: 55px
                    background: $base
                    transition: background 0.5s
                    padding: 15px
                    svg
                        fill: $font-white
                &:last-child
                    margin: 0
                &:hover > div
                    background: $neon-light
        .copyright
            margin-top: 1.5vw
            text-transform: uppercase
            color: $font
            font-weight: 200
            font-size: 12px
            text-align: center
            opacity: 0.7
            span
                color: $neon-light
</style>

<script>
import LinkedInIcon from '@/assets/svg/linkedin.svg'
import InstagramIcon from '@/assets/svg/instagram.svg'
import GithubIcon from '@/assets/svg/github.svg'

export default {
  components: {
    LinkedInIcon,
    InstagramIcon,
    GithubIcon
  },

  computed: {
    NeedReadBlogButton() {
      return this.$route.meta.layout !== 'blog'
    }
  },

  methods: {
    Subscribe() {
      this.$swal.fire({
        title: 'Enter your Email',
        input: 'text',
        inputAttributes: {
          autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: "Subscribe",
        showLoaderOnConfirm: true,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-success mr-5',
          cancelButton: 'btn btn-light'
        },
        preConfirm: (email) => {
          if(!this.validateEmail(email)){
            this.$swal.showValidationMessage("It's not email: try something else!")
            return
          }

          this.$api.post('/api/subscription', {data: {email: email}})
            .then(response => {
              if(response.status === 200)
                this.$notify({
                  type: 'success',
                  title: 'Thank you',
                  text: 'We have to help whis world together!',
                  group: 'app',
                })
              else
                this.ShowError()
            })
            .catch(error => this.ShowError())
        },
        allowOutsideClick: () => !this.$swal.isLoading()
      })
    },

    ShowError() {
      this.$notify({
        type: 'error',
        title: 'Error',
        text: 'Cannot subscribe now, please try it later',
        group: 'app',
      })
    },

    validateEmail(email) {
      const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      return re.test(String(email).toLowerCase())
    }
  }
}
</script>
