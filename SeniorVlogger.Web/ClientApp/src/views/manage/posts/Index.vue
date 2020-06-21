<template>
    <div class="wrapper">
        <router-link tag="button" to="posts/create" class="btn btn-primary">Create New</router-link>
        
        <div class="col-xl-11 col-md-12 m-auto">
            <vue-good-table :columns="columns" :rows="posts"
                :select-options="{ enabled: true, selectOnCheckboxOnly: true, selectionText: 'posts selected'}"
                :search-options="{ enabled: true }">
                <template slot="table-row" slot-scope="props">
                    <span v-if="props.column.field == 'tags'" class="tags">
                        <span class="tag" v-for="(item, index) in props.row.tags" :key="index">
                            {{ item }}
                        </span> 
                    </span>
                    <span v-else-if="props.column.field == 'mailed'" class="check-success">
                        <i class="fas fa-check" v-if="props.row.mailed"></i>
                        <i class="fas fa-times" v-else></i>
                    </span>
                    <span v-else-if="props.column.field == 'scratch'" class="check-success">
                        <i class="fas fa-check" v-if="props.row.scratch"></i>
                        <i class="fas fa-times" v-else></i>
                    </span>
                    <span v-else-if="props.column.field == 'buttons'" class="buttons">
                        <button class="btn btn-sm btn-danger" @click="OnDelete(props.row.id)">
                            <i class="fas fa-trash"></i>
                        </button>
                    </span>
                    <span v-else>
                        {{props.formattedRow[props.column.field]}}
                    </span>
                </template>
            </vue-good-table>
        </div>
    </div>
</template>

<script>

export default {
    data () {
        return {
            posts: [],

            columns: [
                { label: 'Title', field: 'title', sortable: false},
                { label: 'Description', field: 'description', sortable: false },
                { label: 'Category', field: 'category.name' },
                { 
                    label: 'Date', 
                    field: 'publishDate', 
                    dateOutputFormat: 'dd MMM yyyy',
                    dateInputFormat: 'dd.MM.yyyy',
                    type: 'date'
                },
                { label: 'Author', field: 'author.name' },
                { label: 'Tags', field: 'tags', sortable: false, globalSearchDisabled: true },
                { label: 'Mailed', field: 'mailed', sortable: false, globalSearchDisabled: true },
                { label: 'Scratch', field: 'scratch', sortable: false, globalSearchDisabled: true  },
                { label: '', field: 'buttons', sortable: false, globalSearchDisabled: true }
            ]
        }
    }, 

    methods: {
        OnDelete(id) {
            console.log(id)
        }
    },

    async beforeMount() {
        let response = await this.$api.get('/api/blog')
        this.posts = response.data
    }
}
</script>
