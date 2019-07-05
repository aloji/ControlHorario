Vue.component("navbar", {
    template: '#navbar'
});

Vue.component("employees", {
    template: '#employees'
});

Vue.component("login", {
    template: '#login',
    methods: {
        onLogin: function(){
            
        }
    }
});

Vue.component("employees-table-item", {
    template: '#employees-table-item'
});


var app = new Vue({
    el: '#app',
    props: {
        url: {
            type: String,
            default: 'https://localhost:44377/'
        }
    },
    computed: {
        IsLogin: function(){
            return this.adminToken !== null;
        }
    },
    methods: {
        
    },
    data: {
        adminToken: null
    }
});