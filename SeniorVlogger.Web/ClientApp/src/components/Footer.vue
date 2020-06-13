<template>
  <footer class="wrapper">
    <!-- <router-link to="/blog" v-if="this.$nuxt.$data.layoutName!='blog'">  -->
    <router-link to="/blog"> 
      <button
        type="button"
        class="btn">
        Read Blog
      </button>
    </router-link>

    <!-- <button type="button" class="btn" v-else>Subscribe</button> -->

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
  }
}
</script>
