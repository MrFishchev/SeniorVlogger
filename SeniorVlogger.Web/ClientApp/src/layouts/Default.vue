<template>
  <div class="main">
    <slot/>
  </div>
</template>

<script>
export default {
  mounted () {
    document.title = "SeniorVlogger"
  }
}
</script>