import React from 'react';

import Hero from '../components/Hero';
import Carousel from '../components/Carousel';

function AboutPage(props) {
    return (
        <div className='main-container'>
            <Hero title = {props.title} subTitle = {props.subTitle} text = {props.text}/>
            <Carousel/>
        </div>
        
    );
}

export default AboutPage;