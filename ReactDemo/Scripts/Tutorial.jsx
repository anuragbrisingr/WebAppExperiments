const data = [
    { Id: 1, Author: 'Rama Krishna', Text: 'Namaste! React world, this is Rama Krishna.'},
    { Id: 2, Author: 'Raghu Ram', Text: 'Comment by Raghu.' },
    { Id: 3, Author: 'Arjuna Veera', Text: 'Comment.'}
];

class CommentBox extends React.Component {
    render() {
        return (
            <div className="commentBox">Namaste! I'm a comment Box.
                <h1>Comments</h1>
                <CommentList data={this.props.data} />
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

ReactDOM.render(<CommentBox data={data} />, document.getElementById('content'));