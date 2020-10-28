import React from 'react';
import CardInfo from '../components/CardInfo'

function Card(props) {
    return (
        <div className = 'd-inline-block a-card' onClick={() => props.click() }>
            <img className = 'a-card-image' src = {props.item.imgSrc}/> 
            <CardInfo title = {props.item.title} subTitle = {props.item.subTitle} link = {props.item.link}/>
        </div>
    )
}

export default Card;