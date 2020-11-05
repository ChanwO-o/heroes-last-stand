import React from 'react';

import stock from '../assets/images/stock.jpg'
import Card from '../components/Card'
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col'; 

class Carousel extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            items: [
                {
                    id: 0,
                    title: 'Name',
                    subTitle: 'Position',
                    imgSrc: stock,
                    selected: false
                },
                {
                    id: 1,
                    title: 'Name',
                    subTitle: 'Position',
                    imgSrc: stock,
                    selected: false
                },{
                    id: 2,
                    title: 'Name',
                    subTitle: 'Position',
                    imgSrc: stock,
                    selected: false
                },{
                    id: 3,
                    title: 'Name',
                    subTitle: 'Position',
                    imgSrc: stock,
                    selected: false
                },{
                    id: 4,
                    title: 'Name',
                    subTitle: 'Position',
                    imgSrc: stock,
                    selected: false
                },{
                    id: 5,
                    title: 'Name',
                    subTitle: 'Position',
                    imgSrc: stock,
                    selected: false
                },
            ]

        }
    }

    handleCardClick = (id) => {

        let items = [...this.state.items];

        items[id].selected = items[id].selected ? false : true; 

        items.forEach(item => {
            if (item.id !== id) {
                item.selected = false;
            }
        });

        this.setState({
            items
        });
    }

    makeItems = (items) => {
        return items.map(item => {
            return <Card item={item} click = {() => this.handleCardClick(item.id)} key={item.id} />
        })
    }

    render()
    {
        return (
            <Container fluid={true}>
                <Row className = 'justify-content-around'>
                    {this.makeItems(this.state.items)}
                </Row>
            </Container>
        )
    }

}

export default Carousel;