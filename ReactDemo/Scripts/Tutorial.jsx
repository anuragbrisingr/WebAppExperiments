//const data = [
//    { Id: 1, Author: 'Rama Krishna', Text: 'Namaste! React world, this is Rama Krishna.'},
//    { Id: 2, Author: 'Raghu Ram', Text: 'Comment by Raghu.' },
//    { Id: 3, Author: 'Arjuna Veera', Text: 'Comment.'}
//];

class CommentBox extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: [] };
    }
    // XMLHttpRequest API to retrieve data from server.
    // setState is used to set data in the variable on loading of data.
    // componentWillMount() executes immediately AND ONLY ONCE before rendering occurs.
    // So it is not possible to display data whenever we wish to refresh data.
    //componentWillMount() {
    //    const xhr = new XMLHttpRequest();
    //    xhr.open('get', this.props.url, true);
    //    xhr.onload = () => {
    //        const data = JSON.parse(xhr.responseText);
    //        this.setState({ data: data });
    //    };
    //    xhr.send();
    //}
    // ***********************_______________******************______________________**************************
    // To overcome the problem stated above, componentDidMount() is used and
    // a method is called at regular intervals from within componentDidMount() to check
    // for data change/updation within the server.
    loadCommentsFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
    }
    componentDidMount() {
        this.loadCommentsFromServer();
        window.setInterval(
            () => this.loadCommentsFromServer(),
            this.props.shootRequestInterval
        );
    }
    render() {
        return (
            <div className="commentBox">Namaste! I'm a comment Box.
                <h1>Comments</h1>
                <CommentList data={this.state.data} />
                <CommentForm />
            </div>
            );
    }
}

class CommentList extends React.Component {
    render() {
        const commentNodes = this.props.data.map(comment => (
            <Comment author={comment.Author} key={comment.Id}>
                {comment.Text}
            </Comment>
            ));
        return (
            <div className="commentList">
                {commentNodes}
            </div>
            );
    }
}

class CommentForm extends React.Component {
    render() {
        return (
            <div className="commentForm">Namaste! I'm a comment Form.</div>
            );
    }
}

class Comment extends React.Component {
    rawMarkup() {
        const md = new Remarkable();
        const rawMarkup = md.render(this.props.children.toString());
        return { __html: rawMarkup };
    }
    render() {
        return (
            <div className="comment">
                <h2 className="commentAuthor">{this.props.author}</h2>
                <span dangerouslySetInnerHTML={this.rawMarkup()} />
            </div>
            );
    }
}

ReactDOM.render(<CommentBox url="/comments" shootRequestInterval={ 5000} />, document.getElementById('content'));