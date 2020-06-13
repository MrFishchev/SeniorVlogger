<template>
  <div
    id="about"
    class="wrapper container-fluid">
    <div class="content">
      <!-- ABOUT -->
      <div class="block about">
        <h1>About</h1>
        <div class="container">
          <!-- OPTIONS -->
          <div class="best-options slide-in-left">
            <div class="option">
              <ScaleIcon />
              <div class="title">Scalability</div>
              <div class="desc">Use SOLID principles and Design patterns</div>
            </div>
            <div class="option">
              <CleanIcon />
              <div class="title">Clean</div>
              <div class="desc">Beautiful and readable code</div>
            </div>
            <div class="option">
              <SpeedIcon />
              <div class="title">Fast</div>
              <div class="desc">Fastest loading and lowest latency</div>
            </div>
            <div class="option">
              <ResponsiveIcon />
              <div class="title">Responsive</div>
              <div class="desc">Any devices supported</div>
            </div>
          </div>

          <!-- SKILLS -->
          <div class="skills slide-in-right">
            <div class="skill-bars">
              <div class="bar">
                <div
                  class="fill"
                  style="width:99%">
                  <div class="tag"><span>C#</span></div>
                </div>
                <span>99%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:95%">
                  <div class="tag"><span>HTML</span></div>
                </div>
                <span>95%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:95%">
                  <div class="tag"><span>CSS</span></div>
                </div>
                <span>95%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:85%">
                  <div class="tag"><span>JavaScript</span></div>
                </div>
                <span>85%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:75%">
                  <div class="tag"><span>Vue.js</span></div>
                </div>
                <span>75%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:65%">
                  <div class="tag"><span>Angular</span></div>
                </div>
                <span>65%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:55%">
                  <div class="tag"><span>C / C++</span></div>
                </div>
                <span>55%</span>
              </div>
            </div>

            <div class="additional appers-fadein">
              <p>
                <span>.NET</span> WPF, MVVM, Caliburn, EnityFramework, NHibernate, Autofac, MediatR, SignalR, Quartz, MS SQL
              </p>
              <p>
                <span>Web</span> Angular, Vue.js, Nuxt.js MEAN, MEVN, MongoDB, Apollo, GraphQL, PWA, Webpack, Cordova
              </p>
              <p>
                <span>Embedded</span> OpenWRT, Uboot, drivers
              </p>
            </div>
          </div>
        </div>
      </div>

      <!-- PROJECTS -->
      <!-- <div class="block projects" style="height: 300px">
                <h1>Projects</h1>
                <div class="container"></div>
            </div> -->

      <!-- BLOG -->
      <!-- <div class="block" style="height: 300px">
                <h1>BLOG</h1>
                <div class="container"></div>
            </div> -->

      <!-- CONTACT -->
      <div class="block contact">
        <h1>Contact</h1>
        <div class="container">

          <div class="whoami">
            <!-- <img src="@/static/about.jpg" alt=""> -->
            <div class="img"/>
            <div class="title">
              Aleksey Fishchev<br>
              <span>(Sergeevich)</span>
            </div>
            <div class="desc">
              I'm a <strong>Fullstack .NET Developer</strong>. I was born in Russia but live in Minsk.
              I would like to move to Germany. Let's make the world a better place.
              If you want know more about me or <strong>.NET</strong>, then go to my <nuxt-link to="/blog">blog</nuxt-link>.
            </div>
          </div>

          <div class="contact-form">
            <form action="">
              <input
                type="text"
                name="name"
                placeholder="Name"
                required>
              <input
                type="email"
                name="email"
                placeholder="Email"
                required>
              <textarea
                :maxlength="maxMessageLength"
                v-model="message"
                name="message"
                placeholder="Message"
                @keyup="countdown"/>
              <div class="remaining">{{ remainingMessageLength }}</div>
            </form>
            <button
              type="button"
              class="btn">Submit</button>
          </div>

        </div>
      </div>
    </div>
  </div>
</template>

<style lang="sass">
    @import '@/assets/_colors'
    #about
        padding: 0
        margin: 0
        .content
            h1
              animation: slideInLeft 0.5s
              &::after
                animation: slideInRight 1s
            .block.about
                padding-top: calc(53px + 3vw)
                .container
                    display: flex
                    justify-content: space-around
                    .best-options
                        display: flex
                        flex-direction: column
                        align-items: center
                        justify-content: space-around
                        .option
                            text-align: center
                            max-width: 280px
                            margin-bottom: 1.875rem
                            svg
                                fill: $neon-light
                                width: 35%
                            .title
                                color: $font-white
                            .desc
                                color: $font
                                max-width: 200px
                                margin: 0 auto
                            &:last-child
                                margin-bottom: 0
                    .skills
                        width: 50%
                        display: flex
                        flex-direction: column
                        justify-content: space-evenly
                        .skill-bars
                            .bar
                                position: relative
                                display: flex
                                flex-direction: column
                                justify-content: center
                                align-items: flex-start
                                margin-bottom: 30px
                                background: $base-light
                                border-radius: 5px 5px 3px 3px
                                &:nth-child(even) .fill
                                  animation: widthFromZero 1.5s
                                &:nth-child(odd) .fill
                                  animation: widthFromZero 1s
                                .fill
                                    height: 30px
                                    background: $neon-light
                                    border-radius: 5px 5px 3px 3px
                                    .tag
                                        border-radius: 3px 0 0 3px
                                        width: 110px
                                        background: $base-dark
                                        height: 100%
                                        text-align: center
                                        display: flex
                                        justify-content: center
                                        align-items: center
                                        span
                                            color: $font-white
                                            position: relative
                                & > span
                                    position: absolute
                                    left: calc(100% - 40px)
                                    color: $font-white
                                    font-weight: lighter
                        .additional
                            display: flex
                            flex-direction: column
                            justify-content: space-evenly
                            padding: 5px 10px
                            border-top: 1px solid $neon-light
                            border-bottom: 1px solid $neon-light
                            min-height: 180px
                            h4
                                margin-top: 5px
                                margin-bottom: 10px
                                color: $font-white
                                font-size: 1.125rem
                            p
                                padding: 0 15px
                                color: $font
                                margin: 0
                                span
                                    color: $font-white
                                    font-weight: 400
                                &::before
                                    content: '-'
                                    position: relative
                                    left: -5px
            .block.contact
                padding-bottom: 6vw
                .container
                    padding-top: 5vw
                    display: flex
                    justify-content: space-around
                    .whoami
                        max-width: 500px
                        .img
                            margin: 0 auto
                            width: 250px
                            height: 250px
                            background: linear-gradient( rgba(0, 0, 0, 0.25), rgba(0, 0, 0, 0.25) ), url('../assets/about.jpg')
                            background-size: contain
                            border-radius: 50%
                            border: 2px solid $neon-light
                        .title
                            color: $font-white
                            text-align: center
                            font-size: 1.25rem
                            font-weight: 600
                            margin: 1vw 0
                            span
                                font-weight: initial
                                font-size: initial
                        .desc
                            text-align: center
                            max-width: 350px
                            color: $font
                    .contact-form
                        display: flex
                        flex-direction: column
                        justify-content: center
                        min-width: 50%
                        input, textarea
                            display: block
                            width: 100%
                            padding: 10px 15px
                            background: $base-dark
                            border: none
                            color: $font
                            &:hover, &:active
                                border: none
                        textarea
                            min-height: 200px
                        input
                            margin-bottom: 5px
                            min-height: 35px
                        .remaining
                            text-align: right
                            padding: 3px 5px
                            color: $font
</style>

<script>
import ScaleIcon from '@/assets/svg/scale.svg'
import CleanIcon from '@/assets/svg/clean.svg'
import SpeedIcon from '@/assets/svg/speed.svg'
import ResponsiveIcon from '@/assets/svg/responsive.svg'

export default {
  layout: 'default',
  components: {
    ScaleIcon,
    CleanIcon,
    SpeedIcon,
    ResponsiveIcon
  },
  data () {
    return {
      maxMessageLength: 255,
      remainingMessageLength: '',
      message: ''
    }
  },
  methods: {
    countdown () {
      this.remainingMessageLength = this.maxMessageLength - this.message.length
      if (this.remainingMessageLength === this.maxMessageLength) { this.remainingMessageLength = '' }
    }
  }
}
</script>
