<template>
  <div class="p404">
    <div class="bowl">
      <div class="spdline"></div>
      <div class="spider">
        <div class="leg left-1"></div>
        <div class="leg left-2"></div>
        <div class="leg left-3"></div>
        <div class="leg right-1"></div>
        <div class="leg right-2"></div>
        <div class="leg right-3"></div>
      </div>
    </div>
    <div class="ghost">
      <div class="eye eye-left"></div>
      <div class="eye eye-right"></div>
      <div class="chocolate"></div>
      <div class="mouth"></div>
    </div>

    <div class="signwood"></div>
    <div class="signtext" @click="$router.replace('/')">
      <p>
        404
        <br />Page not found
      </p>
    </div>
    <div class="pin"></div>

    <div class="line"></div>
  </div>
</template>

<style lang="sass" scoped>
@import '@/assets/_colors'
$dkblue-2: #cfd8dc
$dkblue-4: #90a4ae
$dkblue-11: #161c1f

$shadow: rgba(0, 0, 0, 0.2)

$signwood-bg: #5d4037
$signwood-brd: #3e2723

$signtext-cl: rgba(0, 0, 0, 0.8)
$signtext-bg: #8a6b47
$signtext-brd: #58442d

.p404 
  margin: 0
  background: var(--base)
  width: 100vw
  height: 100vh


.line 
  position: fixed
  width: 550px
  height: 2px
  background: $dkblue-4
  left: calc(50vw - 275px)
  top: calc(50vh + 73px)


.bowl 
  background: var(--base-light)
  position: fixed
  width: 100px
  height: 45px
  border-radius: 5px 5px 50px 50px
  left: calc(50vw - 25px)
  top: calc(50vh + 19px)
  border: 4px solid var(--base-dark)
  box-shadow: inset -5px 0 0 $shadow
  z-index: 50


.signwood 
  background: $signwood-bg
  position: fixed
  width: 17px
  height: 150px
  border-radius: 5px
  left: calc(50vw - 80px)
  top: calc(50vh - 83px)
  border-top: 4px solid $signwood-brd
  border-left: 4px solid $signwood-brd
  border-bottom: 4px solid transparent
  border-right: 4px solid $signwood-brd
  box-shadow: inset -4px 0 0 $shadow


.signtext 
  cursor: pointer
  background: $signtext-bg
  position: fixed
  width: 125px
  height: 60px
  border-radius: 5px
  left: calc(50vw - 147px)
  top: calc(50vh - 87px)
  transform: rotate(-15deg)
  transform-origin: 60% 0%
  border: 4px solid $signtext-brd
  transition: transform 0.3s ease


.signtext:hover 
  transform: rotate(-20deg)


.signtext > p 
  text-align: center
  margin: 10px 0px 0px 0px
  padding: 0
  font-weight: bold
  font-size: 13px
  font-family: Roboto Slab
  color: $signtext-cl


.pin 
  width: 5px
  height: 5px
  position: fixed
  background: #9e9e9e
  border: 1px solid #424242
  border-radius: 100%
  left: calc(50vw - 70px)
  top: calc(50vh - 79px)


.spider 
  width: 10px
  height: 10px
  background: $dkblue-11
  position: absolute
  border-radius: 100%
  left: 25px
  top: 15px
  transition: transform 0.3s ease


.leg 
  width: 5px
  height: 1px
  position: absolute
  background: $dkblue-11


.left-1 
  margin-left: -4px
  transform: rotate(15deg)
  margin-top: 1px


.left-2 
  margin-left: -4px
  transform: rotate(0deg)
  margin-top: 2px


.left-3 
  margin-left: -4px
  transform: rotate(-15deg)
  margin-top: 2px


.right-1 
  margin-left: 8px
  transform: rotate(-15deg)
  margin-top: -6px


.right-2 
  margin-left: 8px
  transform: rotate(0deg)
  margin-top: 2px


.right-3 
  margin-left: 8px
  transform: rotate(15deg)
  margin-top: 2px


.spdline 
  position: absolute
  background: $dkblue-11
  width: 1px
  height: 20px
  left: 30px


.bowl:hover > .spider 
  transform: translate(0, 5px)


.bowl:hover + .ghost 
  transform: translate(0, -15px)
  opacity: 1


.ghost:hover 
  transform: translate(0, -15px)
  opacity: 1


.ghost 
  z-index: 20
  position: fixed
  height: 65px
  width: 50px
  background: rgba(255, 255, 255, 0.5)
  border-radius: 100px 100px 5px 5px
  left: 15px
  border: 2px solid rgba(255, 255, 255, 0.2)
  left: calc(50vw + 13px)
  top: calc(50vh - 12px)
  opacity: 0.25
  box-shadow: inset -5px 0 0 rgba(0, 0, 0, 0.07)
  transition: opacity 0.5s ease, transform 0.3s ease


.eye 
  position: absolute
  width: 7px
  height: 10px
  border-radius: 100%
  background: rgba(258, 255, 255, 0.5)


.eye-left 
  left: 14px
  top: 15px


.eye-right 
  left: 28px
  top: 15px


.mouth 
  position: absolute
  width: 20px
  height: 13px
  border-radius: 100%
  background: rgba(258, 255, 255, 0)
  top: 21px
  left: 14px
  border-left: 2px solid transparent
  border-right: 2px solid transparent
  border-top: 2px solid transparent
  border-bottom: 2px solid #c9ccce


.chocolate 
  position: absolute
  width: 4px
  height: 7px
  border-radius: 0px 0px 100% 100%
  background: $signwood-bg
  right: 15px
  top: 35px

</style>