<template>
    <div id="unsubscribe">
        <div class="inner">
            <h1>Do you want to unsubscribe?</h1>
            <h4>If you unsubscribe, you will stop receiving our newsletter.</h4>
            <div class="buttons">
                <button type="button" class="btn btn-lg" @click.prevent="Unsubscribe">Unsubscribe</button>
                <button type="button" class="btn btn-lg cancel" @click.prevent="ReturnBack">Cancel</button>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    
    methods: {
        Unsubscribe(){
            this.$api.delete('/api/subscription/'+this.$route.params.id,)
            .then(response => {
              if(response.status === 200){
                  
                  this.$notify({
                    type: 'success',
                    title: 'I will miss you',
                    text: 'You have been unsubsribed from newsletter',
                    group: 'app',
                  })
              }
              else{
                this.ShowError()
                }
            })
            .catch(error => this.ShowError())
            this.ReturnBack()
        },
        ReturnBack(){
            this.$router.push('/blog')
        },
        ShowError(){
            this.$notify({
                type: 'error',
                title: 'Error',
                text: 'Cannot unsubscribe now, please try again later',
                group: 'app',
            })
        }
    }
}
</script>

<style lang="sass" scoped>
    #unsubscribe
        display: flex
        flex-direction: column
        align-items: center
        max-width: 600px
        margin: 0 auto
        position: relative
        top: 10vh
        .inner
            width: 100%
        h1
            font-family: "Rokkitt", sans-serif
            letter-spacing: 0.05em
            font-size: 1.25rem
            line-height: 1.2
            text-align: center
            margin: 0 auto 0.25em
        h4
            color: #777
            letter-spacing: 0.1em
            font-size: 1rem
            line-height: 1.4
            margin: 0 auto 2em
            text-align: center
        .buttons
            width: 100%
            display: flex
            justify-content: center
            flex-wrap: wrap
            align-items: center
            .btn
                display: block
                padding: 10px 30px
                font-size: 1.125rem
                background-color: #d96074
                border: 0
                cursor: pointer
                border-radius: 4px
                letter-spacing: 0.1em
                color: #fff
                margin-right: 20px
                transition: all 0.25s ease-in-out
                &:hover
                    background-color: #cf3746
            .btn.cancel
                margin-right: 0
                color: #333
                background-color: #dfe0ed
                &:hover
                    background-color: #bec5db
</style>