<template>
  <div id="main"
    class="shapes container-fluid vh-100 row">
    <div class="mask"/>
    <div class="title noselect">
      <h3>Hello! I am</h3>
      <h1>Mr. Fishchev</h1>
      <h3 class="job">Senior Software Engineer</h3>
      <div class="routes">
        <router-link to="/blog">
          <button
            type="button"
            class="btn btn-lg">Blog</button>
        </router-link>
        <router-link to="/about">
          <button
            type="button"
            class="btn btn-lg">About</button>
        </router-link>
      </div>
    </div>
    <div class="svg">
      <Html5 />
      <Angular />
      <CSharp />
      <Css3 />
      <Dotnet />
      <Github />
      <Java />
      <Microsoft />
      <Rabbit />
      <Sass />
      <Vue />
      <Bitbucket />
      <Firebase />
      <Git />
      <VisualStudio />
    </div>
  </div>
</template>

<script>
import IconsFinder from '@/assets/scripts/iconsfinder'
import Html5 from '@/assets/svg/html-5.svg'
import Angular from '@/assets/svg/angularjs.svg'
import CSharp from '@/assets/svg/c-sharp-logo.svg'
import Css3 from '@/assets/svg/css3.svg'
import Dotnet from '@/assets/svg/dotnet-color.svg'
import Github from '@/assets/svg/github.svg'
import Java from '@/assets/svg/JavaScript-color.svg'
import Microsoft from '@/assets/svg/microsoft-logo.svg'
import Rabbit from '@/assets/svg/RabbitMQ-color.svg'
import Sass from '@/assets/svg/sass-color.svg'
import Vue from '@/assets/svg/vuejs-color.svg'
import Bitbucket from '@/assets/svg/bitbucket.svg'
import Firebase from '@/assets/svg/firebase.svg'
import Git from '@/assets/svg/git.svg'
import VisualStudio from '@/assets/svg/visual-studio.svg'

export default {
  name: 'MainSlider',
  components: {
    Html5,
    Angular,
    CSharp,
    Css3,
    Dotnet,
    Github,
    Java,
    Microsoft,
    Rabbit,
    Sass,
    Vue,
    Bitbucket,
    Firebase,
    Git,
    VisualStudio
  },
  mounted () {
    IconsFinder.start()
  },
  beforeDestroy () {
    IconsFinder.stop()
  }
}
</script>

<style lang="sass">
    @import '@/assets/_colors'
    #main
        background: var(--base)
        margin: 0
        padding: 0
        .mask
            margin: 0
            padding: 0
            width: 100%
            height: 100%
            z-index: 5
            background: var(--base)
            opacity: 0.3
        .title
            z-index: 10
            text-align: center
            margin: auto
            left: 50%
            top: 50%
            position: absolute
            transform: translateX(-50%) translateY(-50%)
            width: 100%
            background: transparent
            h1
                position: relative
                top: -3px
                font-size: 3.75rem
                font-weight: 100
                color: #fff
                line-height: 1.1
                text-shadow: 0 0 10px var(--neon), 0 0 20px var(--neon), 0 0 30px var(--neon), 0 0 40px var(--neon-light), 0 0 70px var(--neon-light), 0 0 80px var(--neon-light), 0 0 100px var(--neon-light), 0 0 150px var(--neon-light)
            h3
                font-weight: 200
                font-size: 1.75rem
                line-height: 1
                color: #fff
                letter-spacing: 3px
            .job
                font-size: 1.5rem
            .routes
                display: flex
                justify-content: center
                position: relative
                top: 30px
                .btn
                    width: 160px
                    &:first-child
                        margin-right: 1.5vw
                    &:last-child
                        margin-left: 1.5vw
        .svg
            display: none
        svg
            transition: fill .3s ease-out
            display: block
            width: 30px
            height: 30px
            position: absolute
            transform: translateZ(0px)

</style>
