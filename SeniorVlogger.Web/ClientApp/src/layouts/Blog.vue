<template>
  <div class="main" id="blog-layout">
    <Header class="blog"/>
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
  components: {
    Footer,
    Header
  },
  mounted () {
    document.body.classList.add('light')
  },
  destroyed () {
    document.body.classList.remove('light')
  }
}
</script>

<style lang="sass">
  #blog-layout
    .content
      padding-top: 53px
      min-height: calc(100vh - 214px)
</style>
