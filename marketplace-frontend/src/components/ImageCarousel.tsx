import { useKeenSlider } from "keen-slider/react";
import "keen-slider/keen-slider.min.css";

type Props = {
  images: string[];
};

const ImageCarousel = ({ images }: Props) => {
  const [sliderRef] = useKeenSlider({
    loop: true,
    mode: "snap",
  });

  return (
    <div ref={sliderRef} className="keen-slider rounded overflow-hidden">
      {images.map((img, i) => (
        <div key={i} className="keen-slider__slide">
          <img
            src={img}
            alt={`Товар ${i + 1}`}
            className="w-full h-64 object-cover"
          />
        </div>
      ))}
    </div>
  );
};

export default ImageCarousel;
