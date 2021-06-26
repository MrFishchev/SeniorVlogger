<template>
  <div class="main" id="blog-layout" :data-theme="isLightMode ? 'light' : 'dark'">
    <Header @light-mode-changed="lightModeChanged" :is-light-mode="isLightMode" class="blog"/>
    <div class="content">
      <slot />
    </div>
    <Footer />
  </div>
</template>

<script>
import Footer from '@/components/Footer'
import Header from '@/components/Header'

export default {
  data(){
    return{
      isLightMode: true
    }
  },
  components: {
    Footer,
    Header
  },
  mounted () {
    document.title = "SeniorVlogger - Blog"
    
    this.isLightMode = this.$store.getters.IsLightMode
    if(this.isLightMode){
      document.body.classList.add('light')
    }
    else{
      document.body.classList.add('dark')
    }
  },
  
  methods:{
    lightModeChanged(value){
      this.isLightMode = value
      
      if(this.isLightMode){
        document.body.classList.remove('dark')
        document.body.classList.add('light')

        this.$store.dispatch('SET_THEME', 'light')
      }
      else{
        document.body.classList.remove('light')
        document.body.classList.add('dark')
        this.$store.dispatch('SET_THEME', 'dark')
      }
    }
  },
  
  destroyed () {
    if(this.isLightMode){
      document.body.classList.remove('light')
    }
    else{
      document.body.classList.remove('dark')
    }
  }
}
</script>

<style lang="sass">
  #blog-layout
    &[data-theme="dark"]
      span, a
        background: none!important
      .tags
        span
          a
            background-color: #675f6b!important
            color: #e5e3e6
    .content
      padding-top: 53px
      min-height: calc(100vh - 214px)
</style>
