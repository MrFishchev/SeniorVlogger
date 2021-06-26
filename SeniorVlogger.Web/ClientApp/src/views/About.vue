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
              <div class="desc">Use SOLID principles and Clean Architecture</div>
            </div>
            <div class="option">
              <CleanIcon />
              <div class="title">Readibility</div>
              <div class="desc">Beautiful and readable code</div>
            </div>
            <div class="option">
              <SpeedIcon />
              <div class="title">Availability</div>
              <div class="desc">Fastest loading and lowest latency</div>
            </div>
            <div class="option">
              <ResponsiveIcon />
              <div class="title">Crossplatform</div>
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
                  <div class="tag"><span>.NET</span></div>
                </div>
                <span>99%</span>
              </div>
              <div class="bar">
                <div
                    class="fill"
                    style="width:60%">
                  <div class="tag"><span>Go</span></div>
                </div>
                <span>60%</span>
              </div>
              <div class="bar">
                <div
                    class="fill"
                    style="width:70%">
                  <div class="tag"><span>Architecture</span></div>
                </div>
                <span>70%</span>
              </div>
              <div class="bar">
                <div
                    class="fill"
                    style="width:83%">
                  <div class="tag"><span>Docker</span></div>
                </div>
                <span>83%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:75%">
                  <div class="tag"><span>JavaScript</span></div>
                </div>
                <span>75%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:85%">
                  <div class="tag"><span>Vue.js</span></div>
                </div>
                <span>85%</span>
              </div>
              <div class="bar">
                <div
                  class="fill"
                  style="width:97%">
                  <div class="tag"><span>CSS</span></div>
                </div>
                <span>97%</span>
              </div>
            </div>

            <div class="additional appers-fadein">
              <p>
                <span>Backend</span> .NET 5, EntityFramework, Multitenancy, SAGA, Autofac, Quartz, IdentityServer, gRPC, REST
              </p>
              <p>
                <span>Infrastructure</span> Docker, Kubernetes, MSSQL, MongoDB, Redis, RabbitMQ, Monitoring, ELK
              </p>
              <p>
                <span>Frontend</span> Vue.js, Blazor, Nuxt.js, Bootstrap, CSS Grid, Flexbox
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
            <div class="img"/>
            <div class="title">
              Aleksey Fishchev<br>
            </div>
            <div class="desc">
              Hi Friend! I'm a <strong>Senior Software Engineer</strong>.
              Now I'm on my way to becoming a Software Architect. Let's make the World a better place.
              If you want to know about me or <strong>.NET</strong>, then welcome to my <router-link to="/blog">blog</router-link>.
            </div>
          </div>

          <div class="contact-form">
            <form @submit.prevent="SendContact">
              <input
                type="text"
                name="subject"
                v-model="contact.subject"
                placeholder="Subject"
                required>
              <input
                type="text"
                name="email"
                v-model="contact.callbackEmail"
                placeholder="Email"
                required>
              <textarea
                :maxlength="maxMessageLength"
                v-model="contact.message"
                name="message"
                placeholder="Message"
                @keyup="countdown"/>
              <div class="remaining">{{ remainingMessageLength }}</div>
              <button type="submit" class="btn w-100">Get to know</button>
            </form>
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
        background: var(--base)
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
                                fill: var(--neon-light)
                                width: 35%
                            .title
                                color: var(--font-white)
                            .desc
                                color: var(--font)
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
                                background: var(--base-light)
                                border-radius: 5px 5px 3px 3px
                                &:nth-child(even) .fill
                                  animation: widthFromZero 1.5s
                                &:nth-child(odd) .fill
                                  animation: widthFromZero 1s
                                .fill
                                    height: 30px
                                    background: var(--neon-light)
                                    border-radius: 5px 5px 3px 3px
                                    .tag
                                        border-radius: 3px 0 0 3px
                                        width: 110px
                                        background: var(--base-dark)
                                        height: 100%
                                        text-align: center
                                        display: flex
                                        justify-content: center
                                        align-items: center
                                        span
                                            color: var(--font-white)
                                            position: relative
                                & > span
                                    position: absolute
                                    left: calc(100% - 40px)
                                    color: var(--font-white)
                                    font-weight: lighter
                        .additional
                            display: flex
                            flex-direction: column
                            justify-content: space-evenly
                            padding: 5px 10px
                            border-top: 1px solid var(--neon-light)
                            border-bottom: 1px solid var(--neon-light)
                            min-height: 180px
                            h4
                                margin-top: 5px
                                margin-bottom: 10px
                                color: var(--font-white)
                                font-size: 1.125rem
                            p
                                padding: 0 15px
                                color: var(--font)
                                margin: 0
                                span
                                    color: var(--font-white)
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
                            background-size: cover
                            border-radius: 50%
                            border: 2px solid var(--neon-light)
                        .title
                            color: var(--font-white)
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
                            color: var(--font)
                    .contact-form
                        display: flex
                        flex-direction: column
                        justify-content: center
                        min-width: 50%
                        input, textarea
                            display: block
                            width: 100%
                            padding: 10px 15px
                            background: var(--base-dark)
                            border: none
                            color: var(--font)
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
                            color: var(--font)
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
      contact: {
        message: '',
        email: '',
        subject: ''
      }
    }
  },
  methods: {
    countdown () {
      this.remainingMessageLength = this.maxMessageLength - this.contact.message.length
      if (this.remainingMessageLength === this.maxMessageLength) { this.remainingMessageLength = '' }
    },

    SendContact() {
      if(this.contact.message == '' || this.contact.callbackEmail == '' || this.contact.subject == '') return
      if(!this.ValidateEmail(this.contact.callbackEmail)) {
        this.$notify({
          type: 'warning',
          group: 'app',
          title: 'Oops...',
          text: "It doesn't look like email. Please, verify that."
        })
        return
      }

      this.$api.post('/api/email', this.contact)
      .then(() => {
        this.$notify({
          type: 'success',
          group: 'app',
          title: 'Contact Form',
          text: 'Your message has been sent! Thank you!'
        })
        this.contact.subject = ''
        this.contact.callbackEmail = ''
        this.contact.message = ''
      })
      .catch(() => {
        this.$notify({
          type: 'error',
          group: 'app',
          title: 'Contact Form',
          text: 'Cannot sent message. Please, try again later.'
        })
      })
    },

    ValidateEmail(email) {
      const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      return re.test(String(email).toLowerCase())
    }
  }
}
</script>
