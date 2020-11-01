<template>
  <div class="main">
    <Header />
    <slot />
    <Footer />
  </div>
</template>

<script>
import Header from '@/components/Header.vue'
import Footer from '@/components/Footer.vue'

export default {
  components: {
    Header,
    Footer
  },
  mounted () {
    document.title = "SeniorVlogger - About"
  }
}
</script>
