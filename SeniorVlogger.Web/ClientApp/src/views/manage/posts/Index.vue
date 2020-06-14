<template>
    <div class="wrapper">
        <router-link tag="button" to="posts/create" class="btn btn-primary">Create New</router-link>
        <table v-if="posts && posts.length">
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Tags</th>
                    <th>Date</th>
                    <th/>
                </tr>
            </thead>
            <tbody>
                <tr v-for="post in posts" :key="post.id">
                    <td>{{ post.title }}</td>
                    <td>{{ post.tags }}</td>
                    <td>{{ post.date }}</td>
                    <td>
                        <nuxt-link :to="'/posts/' + post.slug">
                            View
                        </nuxt-link>
                        <router-link :to="'/manage/posts/update/' + post.id" :prefetch="false">
                            Edit
                        </router-link>
                        <a href @click.prevent="removePost(post.id)">
                            Delete
                        </a>
                    </td>
                </tr>
            </tbody>
        </table>
        <p v-else>There are no posts</p>
    </div>
</template>

<script>

export default {
    // middleware: ['auth'],

    data () {
        return {
            posts: []
        }
    },
   
    methods: {
        async removePost (id) {
            
        }
    }    
}
</script>
