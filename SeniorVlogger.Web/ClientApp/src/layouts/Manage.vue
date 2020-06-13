<template>
  <div class="main">
    <slot />
  </div>
</template>

<script>
export default {
  mounted () {
    document.body.classList.add('light')
  },
  destroyed () {
    document.body.classList.remove('light')
  }
}
</script>
