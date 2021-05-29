<template>
    <div class="wrapper p-5 col-xl-11 col-md-12 m-auto">
        <div class="data p-0 border border-darken-2">
            <vue-good-table :columns="columns" :rows="subscribers"
                :select-options="{ enabled: true, selectOnCheckboxOnly: true, selectionText: 'posts selected'}"
                :search-options="{ enabled: true }">
                <template slot="table-row" slot-scope="props">
                    <span v-if="props.column.field === 'isSubscribed'" class="check-success">
                        <i class="fas fa-check text-success" v-if="props.row.isSubscribed"></i>
                        <i class="fas fa-times text-danger" v-else></i>
                    </span>
                    <span v-else-if="props.column.field === 'unsubscribeDate'">
                        <span v-if="props.row.unsubscribeDate">{{props.formattedRow[props.column.field]}}</span>
                        <i class="fas fa-times text-danger" v-else></i>
                    </span>
                    <span v-else-if="props.column.field === 'buttons'" class="table-buttons">
                        <button class="btn btn-sm btn-danger" @click="OnDelete(props.row.id)">
                            <i class="fas fa-trash"></i>
                        </button>
                    </span>
                    <span v-else>
                        {{props.formattedRow[props.column.field]}}
                    </span>
                </template>
            </vue-good-table>

            <div class="controls">

            </div>
        </div>
    </div>
</template>

<script>

export default {
    data () {
        return {
            subscribers: [],

            columns: [
              { label: 'Email', field: 'email'},
              { label: 'Subscribed', field: 'isSubscribed', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' },
              { 
                label: 'From', 
                field: 'subscribeDate',
                dateInputFormat: 'yyyy-MM-dd\'T\'HH:mm:ss.SSSSSSS',
                dateOutputFormat: 'dd.MM.yyyy',
                type: 'date',
                tdClass: 'vgt-left-align',
                thClass: 'vgt-left-align'
              },
              {
                label: 'Unsubscribed',
                field: 'unsubscribeDate',
                dateInputFormat: 'yyyy-MM-dd\'T\'HH:mm:ss.SSSSSSS',
                dateOutputFormat: 'dd.MM.yyyy',
                type: 'date',
                tdClass: 'vgt-left-align',
                thClass: 'vgt-left-align'
              },
              { label: '', field: 'buttons', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' }
            ]
        }
    }, 

    methods: {
        async OnDelete(id) {
            let res = await this.$swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to restore the subscriber!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                customClass: {
                    confirmButton: 'btn btn-danger mr-3',
                    cancelButton: 'btn btn-light'
                },
                buttonsStyling: false
            })

            if(res.isConfirmed){
                let subscriber = this.subscribers.find(x=> x.id === id)
                let encodedEmail = btoa(subscriber.email)
                let response = await this.$api.delete(`/api/subscription/${encodedEmail}`)
                if(response.status === 200){
                    this.$notify({
                        type: 'success',
                        text: 'Subscriber was deleted',
                        group: 'app',
                    })
                    await this.LoadSubscribers()
                }
                else{
                    this.$notify({
                        type: 'error',
                        title: 'Error',
                        text: 'Cannot delete subscriber',
                        group: 'app',
                    })
                }
               
            }
        },

        async LoadSubscribers() {
            let response = await this.$api.get('/api/subscription')
            this.subscribers = response.data
        }
    },

    async beforeMount() {
        await this.LoadSubscribers()
    }
}
</script>

<style lang="sass">
    .wrapper
        td.center-item
            text-align: center
            vertical-align: middle
        & > .buttons
            display: flex
            justify-content: flex-end
            padding-bottom: 20px
        .data
            .table-buttons
                display: flex
                justify-content: center
            .controls
                background: linear-gradient(#f4f5f8,#f1f3f6)
                height: 40px
        .tag
          display: inline-block
          background: #e0ecff
          padding: 5px
          margin: 2px
          color: #8c8c8c

</style>
